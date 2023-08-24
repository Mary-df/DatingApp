import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers()
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  getUsers() {
    this.http.get(environment.apiUrl + 'users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log('Completato'),
    }); //ritorna un observable
  }

  cancelRegistrMode(event: boolean) {
    this.registerMode = event;
  }

}
