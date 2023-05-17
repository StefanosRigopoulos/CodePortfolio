import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Photo } from 'src/app/_models/photo';
import { Project } from 'src/app/_models/project';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ProjectsService } from 'src/app/_services/projects.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-project-edit',
  templateUrl: './project-edit.component.html',
  styleUrls: ['./project-edit.component.css']
})
export class ProjectEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  project: Project | undefined;
  model: any = {};

  pieces: string[] = [];
  username: string;
  projectname: string;

  uploader: FileUploader | undefined;
  baseUrl = environment.apiUrl;
  user: User | null = null;

  constructor(private accountService: AccountService, private projectService: ProjectsService, private router: Router, private toastr: ToastrService)
  {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user
      }
    })
  }

  ngOnInit(): void {
    this.pieces = this.router.url.split('/');
    this.username = this.pieces[1];
    this.projectname = this.pieces[3];
    this.loadProject(this.username, this.projectname);
    this.initializeUploader();
  }

  loadProject(username: string, projectName: string) {
    if (!username || !projectName) return;
    this.projectService.getProject(username, projectName).subscribe({
      next: project => {
        this.model = project,
        this.project = project
      }
    })
  }

  updateProject() {
    this.projectService.updateProject(this.username, this.projectname, this.editForm?.value).subscribe({
      next: _ => {
        this.toastr.success("Project updated successfully!");
        this.editForm?.reset(this.project);
        this.router.navigateByUrl('/' + this.user.username + '/p/' + this.project.projectName);
      }
    });
  }

  deleteProject() {  //TODO
    this.projectService.deleteProject(this.username, this.projectname).subscribe({
      next: _ => {
        this.toastr.success("Project deleted successfully!");
        this.router.navigateByUrl('/' + this.user.username);
      }
    });
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'projects/' + this.username + '/' + this.projectname + '/add-project-photo',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.project?.projectPhotos.push(photo);
        // If there is any problem check (vid 149) at 13:20 and onwards
      }
    }
  }

  setMainProjectPhoto(photo: Photo) {
    this.projectService.setMainProjectPhoto(this.username, this.projectname, photo.id).subscribe({
      next: () => {
        if (this.project) {
          this.project.mainPhotoURL = photo.url;
          this.project.projectPhotos.forEach(p => {
            if (p.isMain) p.isMain = false;
            if (p.id === photo.id) p.isMain = true;
          })
        }
      }
    })
  }

  deleteProjectPhoto(photoId: number) {
    this.projectService.deleteProjectPhoto(this.username, this.projectname, photoId).subscribe({
      next: () => {
        if (this.project) {
          this.project.projectPhotos = this.project.projectPhotos.filter(x => x.id != photoId)
        }
      }
    })
  }
}
