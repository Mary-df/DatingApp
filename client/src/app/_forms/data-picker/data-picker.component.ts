import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { listLocales } from 'ngx-bootstrap/chronos';

@Component({
  selector: 'app-data-picker',
  templateUrl: './data-picker.component.html',
  styleUrls: ['./data-picker.component.css']
})
export class DataPickerComponent implements ControlValueAccessor{
  @Input() label = '';
  @Input() maxDate: Date | undefined;
  @Input() type = 'date';
  bsConfig: Partial<BsDatepickerConfig> | undefined;
  //Partial rede tutte le propietà della classe che gli passo facoltative
  //in questo caso lòa classe che gli passo è BsDatepickerConfig
  /* locale = 'it';
  locales = listLocales();
 */
  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
    //this.localeService.use(this.locale);
    this.bsConfig = {
      containerClass: "theme-red",
      dateInputFormat: 'DD MMMM YYYY',
    }
   }

  writeValue(obj: any): void { }

  registerOnChange(fn: any): void { }

  registerOnTouched(fn: any): void { }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }
}
