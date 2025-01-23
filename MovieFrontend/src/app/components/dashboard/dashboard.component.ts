import { Component, OnInit, AfterViewInit } from '@angular/core';
import { MovieService } from '../../services/Movies/movie.service';
import { CommonModule } from '@angular/common';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, Subject, switchMap, distinctUntilChanged } from 'rxjs';
import * as bootstrap from 'bootstrap';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit, AfterViewInit {
  MoviesList: any;
  selectedFile: File | null = null;
  selectedUpdateFile: File | null = null;
  movieToUpdateId: number | null = null;
  movieToDelete: number | null = null;
  searchQuery: string = '';
  filteredMovies: any[] = [];
  apikey = sessionStorage.getItem("ApiKey")!
  private searchSubject = new Subject<string>();
  isAdmin : boolean = true;
  userRole: string | null = null;
  modal!: bootstrap.Modal;

  constructor(
    private movieservice: MovieService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.userRole = sessionStorage.getItem("Role");
    this.getAllMovie();
    this.isAdminFn();
    this.searchSubject
    .pipe(
      debounceTime(300),  
      distinctUntilChanged(), 
      switchMap((query) => {
        if (!query) {
          return this.movieservice.GetMovie(); 
        }
        return this.movieservice.SearchMovie(query, this.apikey);  
      })
    )
    .subscribe({
      next: (response) => {
        this.MoviesList = response.data || [];
        console.log("object", this.MoviesList);
      },
      error: (err) => {
        this.getAllMovie();
      },
    });
  
  }

  onSearchChange(query: string) {
    this.searchSubject.next(query); 
  }

  ngAfterViewInit(): void {
    const modalElement = document.getElementById('addMovieModal') as HTMLElement;
    if (modalElement) {
      this.modal = new bootstrap.Modal(modalElement, { keyboard: false }); // Ensures proper initialization
    }
  }  

  isAdminFn(): boolean {
    console.log("Role",this.userRole )
    if (this.userRole === "Admin") {
      this.isAdmin = true;
      return true;
    } else {
      this.isAdmin = false;
      return false;
    }
  }

  AddMovieForm = new FormGroup({
    Title: new FormControl('', [Validators.required]),
    ReleaseYear: new FormControl('', [Validators.required]),
    PosterImg: new FormControl('', [Validators.required]),
  });

  UpdateMovieForm = new FormGroup({
    Title: new FormControl('', [Validators.required]),
    ReleaseYear: new FormControl('', [Validators.required]),
    PosterImg: new FormControl('', [Validators.required]),
  });

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      this.AddMovieForm.patchValue({ PosterImg: file });
      this.AddMovieForm.get('PosterImg')?.updateValueAndValidity(); 
    }
  }  

  onUpdateFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedUpdateFile = file;
      this.UpdateMovieForm.patchValue({ PosterImg: file });
      this.UpdateMovieForm.get('PosterImg')?.updateValueAndValidity();
    }
  }

  openUpdateModal(movie: any) {
    this.movieToUpdateId = movie.movieId;
    this.UpdateMovieForm.patchValue({
      Title: movie.title,
      ReleaseYear: movie.releaseYear,
    });
  }

  setMovieToDelete(movieId: number) {
    this.movieToDelete = movieId;
  }

  UpdateMovie() {
    console.log('Update Movie Called');
    console.log(this.movieToUpdateId);

    if (this.UpdateMovieForm.valid && this.movieToUpdateId !== null) {
      const formData = new FormData();
      formData.append('Title', this.UpdateMovieForm.get('Title')?.value || '');
      formData.append(
        'ReleaseYear',
        this.UpdateMovieForm.get('ReleaseYear')?.value || ''
      );
      if (this.selectedUpdateFile) {
        formData.append('PosterImg', this.selectedUpdateFile);
      }

      this.movieservice.UpdateMovie(this.movieToUpdateId, formData).subscribe({
        next: (data) => {
          if (data.status === 200) {
            this.toastr.success(data.message, 'Success');
            this.getAllMovie();
          } else {
            this.toastr.error(data.message, 'Error');
          }
          this.movieToUpdateId = null;
        },
      });
    } else {
      this.UpdateMovieForm.markAllAsTouched();
    }
  }

  hideModal() {
    if (this.modal) {
      this.modal.hide(); // Use the modal instance to hide it
    }
  }
  
  

  AddMovie() {
    console.log(this.AddMovieForm.value);
    if (this.AddMovieForm.valid) {
      const formData = new FormData();
      formData.append('Title', this.AddMovieForm.get('Title')?.value || '');
      formData.append(
        'ReleaseYear',
        this.AddMovieForm.get('ReleaseYear')?.value || ''
      );
      if (this.selectedFile) {
        formData.append('PosterImg', this.selectedFile);
      }

      this.movieservice.AddMovie(formData).subscribe({
        next: (data) => {
          if (data.status == 200) {
            this.toastr.success(data.message, 'Success');
            // const modalElement = document.getElementById('addMovieModal') as HTMLElement;
            // console.log(modalElement);  // Log to see if the modal is found
            
            // if (modalElement) {
            //   const modal = new bootstrap.Modal(modalElement);
            //   modal.hide();
            // }
            this.hideModal();
          } else {
            this.toastr.error(data.message, 'Error');
          }
          this.getAllMovie();
        },
      });
    } else {
      this.AddMovieForm.markAllAsTouched();
    }
  }

  DeleteMovie(movieId: number) {
    console.log(movieId)
    if (movieId) {
      this.movieservice.DeleteMovie(movieId).subscribe({
        next: (data) => {
          if (data.status === 200) {
            this.toastr.success(data.message, 'Success');
            this.getAllMovie();
          } else {
            this.toastr.error(data.message, 'Error');
          }
        },
        error: (err) => {
          this.toastr.error('Failed to delete the movie', 'Error');
          console.error(err);
        },
      });
    }
  }
  

  getAllMovie() {
    this.movieservice.GetMovie().subscribe({
      next: (data) => {
        this.MoviesList = data.data;
      },
    });
  }
}
