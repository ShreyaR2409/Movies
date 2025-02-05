import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../services/Users/user.service';
import { ToastrService } from 'ngx-toastr';
import { JwtHelperService } from "@auth0/angular-jwt";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  constructor(private userservice: UserService, private toastr: ToastrService, private router: Router) { }
  LoginForm = new FormGroup({
    Email: new FormControl('', [Validators.required, Validators.email]),
    Password: new FormControl('', Validators.required)
  })

  Login() {
    if (this.LoginForm.valid) {
      this.userservice.UserLogin(this.LoginForm.value).subscribe({
        next: (data => {
          if (data.status == 200) {
            this.toastr.success(data.message, 'Success');
            this.router.navigateByUrl('/dashboard');
            sessionStorage.setItem("Token", data.token);
            // const helper = new JwtHelperService();
            // const decodedToken = helper.decodeToken(data.token)
            // console.log(decodedToken);
            // sessionStorage.setItem("Token",data.token);
            // sessionStorage.setItem("Id", decodedToken.Id);
            // sessionStorage.setItem("Email", decodedToken.Email);
            // sessionStorage.setItem("Role", decodedToken.Role);
            // sessionStorage.setItem("ApiKey", decodedToken.Api);
          }
          else {
            this.toastr.error(data.message, 'error');
          }
        })
      })
    }
    else {
      this.LoginForm.markAllAsTouched();
    }
  }
}
