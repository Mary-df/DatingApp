import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  //currentUser$ : Observable <User | null> = of(null);
  //il $ è una convenzione di rxjs per indicare che quella variale è un Observable

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    //this.currentUser$ = this.accountService.currentUser$;
  }

  /* getCurrentUser(){
    this.accountService.currentUser$.subscribe({
      //next: user => this.loggedIn = !!user,
      error: error => console.log(error),
    })   
    //currentUser$ --> è il nome dell'observable
    //!!user --> il !! trasforma il mio oggetto user in un buleano
  } */

  login() {
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
      },
      error: error => console.log(error),
    })
  }

  logout() {
    this.accountService.logout();
  }

}
