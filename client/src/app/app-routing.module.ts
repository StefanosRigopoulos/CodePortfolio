import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { ExploreComponent } from './explore/explore.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { ProjectDetailsComponent } from './projects/project-details/project-details.component';
import { MessagesComponent } from './messages/messages.component';
import { RegisterComponent } from './register/register.component';
import { ResetComponent } from './reset/reset.component';
import { ProjectEditComponent } from './projects/project-edit/project-edit.component';
import { ProjectCreationComponent } from './projects/project-creation/project-creation.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesMemberGuard } from './_guards/prevent-unsaved-changes-member.guard';
import { PreventUnsavedChangesProjectGuard } from './_guards/prevent-unsaved-changes-project.guard';
import { PreventUnsavedChangesCreateGuard } from './_guards/prevent-unsaved-changes-create.guard';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { AdminGuard } from './_guards/admin.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'reset', component: ResetComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'errors', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'admin', component: AdminPanelComponent, canActivate: [AdminGuard]},
      {path: 'explore', component: ExploreComponent},
      {path: 'messages', component: MessagesComponent},
      {path: ':username', component: MemberDetailComponent, resolve: {member: MemberDetailResolver}},
      {path: ':username/create', component: ProjectCreationComponent, canDeactivate: [PreventUnsavedChangesCreateGuard]},
      {path: ':username/edit', component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesMemberGuard]},
      {path: ':username/p/:projectname', component: ProjectDetailsComponent},
      {path: ':username/p/:projectname/edit', component: ProjectEditComponent, canDeactivate: [PreventUnsavedChangesProjectGuard]}
    ]
  },
  {path: '**', component: NotFoundComponent, pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
