import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { Member } from '../_models/member';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {};
  //currentUser$ : Observable <User | null> = of(null);
  //il $ è una convenzione di rxjs per indicare che quella variale è un Observable

  constructor(public accountService: AccountService, private router: Router, private toast: ToastrService) { }

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

 /*  login() {
    this.accountService.login(this.model).subscribe({
      next: response => {
        this.router.navigateByUrl('/members');
        console.log(response);
      },
      error: error => console.log(error),
    })
  } */
  //PER RENDERLO PIU' ORDINATO POSSIAMO
  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => this.router.navigateByUrl('/members'),
      //error: error => this.toast.error(error.error),
      //qui posso togliere gli errori perchè ora lo gestimo con gli interceptor
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
