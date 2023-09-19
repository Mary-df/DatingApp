import { Component, EventEmitter, Input, OnInit, Output, forwardRef } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormGroup, NG_VALUE_ACCESSOR, ValidatorFn, Validators } from '@angular/forms';
import { TextInputComponent } from '../_forms/text-input/text-input.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TextInputComponent),
      multi: true,
    }
  ]
})
export class RegisterComponent implements OnInit {
  //@Input() usersFromHomeComponent: any; +
  //  --> questa va tolta perchè i dati ci arrivano dall'observalr messo in account
  //@Input() avviso il componente che sta per ricevere
  //dati da un componente padre
  @Output() cancelRegister = new EventEmitter();
  //@Output() avvisa il componente che deve inviare
  //dati a un componente padre
  //model: any = {}



  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) { }

  /* register() {
    this.accountService.register(this.model).subscribe({
      next: () => {
        this.cancel();
      },
      error: error => console.log(error),
    });
  } */

  cancel() {
    this.cancelRegister.emit(false);
  }


  /************** FORM REATTIVO ***********************/
  //-> tiene traccia del valore dello stato di validità
  //registerForm: FormGroup | undefined;
  //registerform così mi da errore perchè dobbiamo impostarelo ad un oggetto vuoto del formGroup
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  validationErrors: string[] | undefined;

  ngOnInit(): void {
    this.inizializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18);
  }

  inizializeForm() {
    /* this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required,),
      password: new FormControl('', [Validators.required, Validators.maxLength(8), Validators.minLength(4)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValue('password')])
    }); */
    //con i servizio FormBuild diventa
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.maxLength(8), Validators.minLength(4)]],
      confirmPassword: ['', [Validators.required, this.matchValue('password')]]
    });

    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });    
  }

  //ValidatorFn e ciò che mi voglio far restituire
  matchValue(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : { notMatching: true };
      //{notMatching: true} --> creo un'oggetto
    }
  }

  register() {
    //console.log(this.registerForm?.value);
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    const values = {...this.registerForm.value, dateOfBirth: dob};
    console.log(values);
    this.accountService.register(values).subscribe({
      next: () => {
      this.router.navigateByUrl('/members')
      },
      error: error => {
        //console.log(error);
        this.validationErrors = error;
      }
    });
    
  }

  private getDateOnly(dob: string | undefined) {
    if(!dob) return;
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())).toISOString().slice(0,10);

  }

}
