import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class AccountService {
  baseUrl = environment.apiUrl;
  //baseUrl = "https://localhost:5001/api/";
  private currentUserSource = new BehaviorSubject<User | null>(null);
  //new BehaviorSubject<User | null> gli sto dicento che il tipo può essere sia di tipo User che di tipo null
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  
  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map( (resp: User)=> {
        const user = resp;
        if (user) this.setCurrentUser(user);
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if (user) this.setCurrentUser(user);
      })
    );
  }

  setCurrentUser(user: User){
    localStorage.setItem("user", JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

}
