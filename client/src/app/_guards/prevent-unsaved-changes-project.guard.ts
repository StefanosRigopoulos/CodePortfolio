import { Injectable } from '@angular/core';
import { ProjectEditComponent } from '../projects/project-edit/project-edit.component';
import { CanDeactivate } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesProjectGuard implements CanDeactivate<ProjectEditComponent> {
  canDeactivate(component: ProjectEditComponent): boolean {
    if (component.editForm?.dirty) {
      return confirm('You have unsaved changes! If you continue, any unsaved changes will be lost!');
    }
    return true;
  }
}
