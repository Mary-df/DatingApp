import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent {
  error: any;
  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.['error'];
    //nettiamo i ? perchè potrebbero essere undefine 
    //il ? in typescript sta ad indicare che quel campo è opzionale
    //['error']--> lo trovi dentro interceptor  case 500: {state: {error: ....}}
  }

}
