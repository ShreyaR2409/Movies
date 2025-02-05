import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
@Injectable({
  providedIn: 'root'
})
export class JwtService {

  constructor() { }

  getRole(){
    const token = sessionStorage.getItem("Token");
    if(token){
      const decodedtoken : any = jwtDecode(token);
      return decodedtoken.Role;
    }
  } 

  getApiKey(){
    const token = sessionStorage.getItem("Token");
    if(token){
      const decodedtoken : any = jwtDecode(token);
      return decodedtoken.Api;
    }
  }

  getEmail(){
    const token = sessionStorage.getItem("Token");
    if(token){
      const decodedToken : any = jwtDecode(token);
      return decodedToken.Email;
    }
  }

  getId(){
    const token = sessionStorage.getItem("Token");
    if(token){
      const decodedToken : any = jwtDecode(token);
      return decodedToken.Id;
    }
  }

}
