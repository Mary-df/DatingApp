import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  //@Input() usersFromHomeComponent: any; +
  //  --> questa va tolta perchè i dati ci arrivano dall'observalr messo in account
  //@Input() avviso il componente che sta per ricevere
  //dati da un componente padre
  @Output() cancelRegister = new EventEmitter();
  //@Output() avvisa il componente che deve inviare
  //dati a un componente padre
  model: any = {}

  constructor(private accountService: AccountService, private toast: ToastrService) { }


  register() {
    this.accountService.register(this.model).subscribe({
      next: () => {
        this.cancel();
      },
      error: error => console.log(error),
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
