import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../services/Users/user.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  todayDate = new Date().toISOString().split('T')[0];
  constructor(
    private userservice: UserService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  RegistrationForm = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    dateOfBirth: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required,Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.pattern('^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{6,}$'),
    ]),
    // confirmPassword: new FormControl('', Validators.required),
    roleId: new FormControl(null, Validators.required),
  });

  registerUser() {
    console.log(this.RegistrationForm.value);
    if (this.RegistrationForm.valid) {
      this.userservice.createUser(this.RegistrationForm.value).subscribe({
        next: (data) => {
          if (data.status == 200) {
            this.toastr.success(data.message, 'Success');
            this.router.navigateByUrl('/login');
          } else {
            this.toastr.error(data.message, 'Error');
          }
        },
      });
    } else {
      this.RegistrationForm.markAllAsTouched();
    }
  }
}
