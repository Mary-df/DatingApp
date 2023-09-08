import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
  if(component.formEdit?.dirty) {
    return confirm('sei sicuro di voler abbandonare la pagina?')
  }
  return true;
};
