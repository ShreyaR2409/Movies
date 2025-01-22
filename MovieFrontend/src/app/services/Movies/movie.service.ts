import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  constructor(private http : HttpClient) { }

  private url = 'https://localhost:7239/api/Movie/';

  public AddMovie(FormData : any): Observable<any>{
    return this.http.post(`${this.url}Add-Movie`,FormData);
  }

  public UpdateMovie(id : number , FormData : any):Observable<any>{
    return this.http.put(`${this.url}Update-Movie?Id=${id}`,FormData);
  }

  public DeleteMovie(id : any) : Observable<any>{
    return this.http.delete(`${this.url}Delete-Movie`);
  }

  public GetMovie():Observable<any>{
    return this.http.get(`${this.url}Get-Movie`);
  }

  public SearchMovie(movie : string , key : string):Observable<any>{
    return this.http.get(`${this.url}getSearchedMovie?s=${movie}&apikey=${key}`)
  }

}
