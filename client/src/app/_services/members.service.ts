import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = []

  constructor(private http: HttpClient) { }

  getMembers() {
    if(this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    );
    //users è il controllore che useremo
  }

  getMember(username: string) {
    const member = this.members.find(x => x.userName === username);
    if(member) return of(member); 
    return this.http.get<Member>(this.baseUrl + 'users/' + username)
  }

  updateMember(member: Member){
    return this.http.put(this.baseUrl + 'users/', member).pipe(
      //non fa uscire lo spinner ogni volta che carichi la pagina profilo 
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index], ...member}
      })
    );
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
 