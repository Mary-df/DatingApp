import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {
  //aggiungo il decoratore Input per passare o dati al tag input
  @Input() label = '';
  @Input() type = 'text';
  @Input() name = ' ';
  //ControlValueAccessor crea un elemento nativo nel dom
  constructor(@Self() public ngControl: NgControl) { 
    this.ngControl.valueAccessor = this;
  }
  //NgControl è una classe che estende tutte le direttive sui controll del modulo

  writeValue(obj: any): void { }

  registerOnChange(fn: any): void { }

  registerOnTouched(fn: any): void {

  }

  //devo aggiungere questo metodo perchè con la modalità stip ho errore 
  //in HTML perchè ngControl.control è null
  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }

}
