import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly url = 'https://localhost:7239/api/User/';
  constructor(private http : HttpClient) { }

  public createUser(UserData : any) : Observable<any>{
     return this.http.post(`${this.url}Register`, UserData);
  }

  public UserLogin(LoginDetails : any) : Observable<any>{
    return this.http.post(`${this.url}Login`,LoginDetails);
  }
}
