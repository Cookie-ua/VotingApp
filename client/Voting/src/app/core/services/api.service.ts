import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  get(path: string, params: HttpParams = new HttpParams()): Observable<any>{
    return this.http.get(`${environment.api_url}${path}`, { params });
  }

  post(path: string, body: Object = {}): Observable<any>{
    return this.http.post(`${environment.api_url}${path}`, body);
  }
  
  put(path: string, body: Object = {}){
    return this.http.put(`${environment.api_url}${path}`, body);
  }

  delete(path: string){
    return this.http.delete(`${environment.api_url}${path}`);
  }
}