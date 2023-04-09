import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { ProjectCreationComponent } from '../projects/project-creation/project-creation.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesCreateGuard implements CanDeactivate<ProjectCreationComponent> {
  canDeactivate(component: ProjectCreationComponent): boolean {
    if (component.projectForm?.dirty) {
      return confirm('You have unsaved changes! If you continue, any unsaved changes will be lost!');
    }
    return true;
  }
}
