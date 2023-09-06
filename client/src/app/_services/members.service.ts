import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl

  constructor(private http: HttpClient) { }

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users')
    //users è il controllore che useremo
  }

  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + 'users/' + username)
  }

  /* QUESTO VA TOLTO E MODIFICATO COME SOPRA PERCHE' ABBIAMO AGGIUNTO
    INTERCEPTOR JWT
  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users', this.getHttpOptions())
    //users è il controllore che useremo
  }

  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + 'users/' + username, this.getHttpOptions())
  }

 
  getHttpOptions() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user = JSON.parse(userString);
    return {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + user.token
        
        //'Bearer ' --> lo spazio affianco a beare è molto importante
                      se non viene messo la bearer verrà unito al token
                      e non verrà convalidato
        
      })
    }
  }*/

}
 