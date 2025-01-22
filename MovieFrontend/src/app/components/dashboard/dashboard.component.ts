import { Component, OnInit } from '@angular/core';
import { MovieService } from '../../services/Movies/movie.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent  implements OnInit {
MoviesList : any;
constructor(private movieservice : MovieService){}

ngOnInit(): void {
  this.getAllMovie();
}
getAllMovie(){
  this.movieservice.GetMovie().subscribe({
    next : (data =>{
      this.MoviesList = data.data
    })
  })
}
}
