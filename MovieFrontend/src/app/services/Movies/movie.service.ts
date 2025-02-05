import { HttpClient, HttpParams } from '@angular/common/http';
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
    return this.http.delete(`${this.url}Delete-Movie?MovieId=${id}`);
  }

  public GetMovie():Observable<any>{
    return this.http.get(`${this.url}Get-Movie`);
  }

  // public GetMovieByPage(pageIndex : number, pageSize: number, sortColumn : string, sortOrder : string, filterValue: string):Observable<any>{
  //   return this.http.get(`${this.url}Get-MovieByPage?PageIndex=${pageIndex}&PageSize=${pageSize}&SortColumn=${sortColumn}&SortOrder=${sortOrder}&FilterValue=${filterValue}`);
  // }

//Another way to pass the query parameter
public GetMovieByPage(pageIndex : number, pageSize: number, sortColumn : string, sortOrder : string, filterValue: string):Observable<any>{
  return this.http.get(`${this.url}Get-MovieByPage`,{
    params : new HttpParams()
    .set('PageIndex',pageIndex)
    .set('PageSize',pageSize)
    .set('SortColumn',sortColumn)
    .set('SortOrder',sortOrder)
    .set('FilterValue',filterValue)
  });
}

  public SearchMovie(movie : string , key : string):Observable<any>{
    return this.http.get(`${this.url}getSearchedMovie?s=${movie}&apikey=${key}`)
  }

}
