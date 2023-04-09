import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { delay, take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Project } from 'src/app/_models/project';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { ProjectsService } from 'src/app/_services/projects.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-project-creation',
  templateUrl: './project-creation.component.html',
  styleUrls: ['./project-creation.component.css']
})
export class ProjectCreationComponent implements OnInit {
  @ViewChild('projectForm') projectForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any) {
  if (this.projectForm?.dirty) {
    $event.returnValue = true;
  }
}
model: any = {};
member: Member;
user: User | null = null;
baseUrl = environment.apiUrl;

projectname: string;
username: string;

uploader: FileUploader | undefined;
HasBaseDropZoneOver: boolean;

constructor(private accountService: AccountService, private memberService: MembersService, private projectService: ProjectsService, private toastr: ToastrService, private router: Router) {
  this.accountService.currentUser$.pipe(take(1)).subscribe({
    next: user => this.user = user
  })
}

  ngOnInit(): void {
    this.loadMember();
    this.initializeUploader();
  }

  loadMember() {
    if(!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: member => {
        this.member = member;
        this.username = member.userName;
      }
    })
  }

  createProject() {
    this.projectname = this.model.projectTitle.replaceAll(" ", "_").toLowerCase();
    this.model.projectName = this.projectname;

    this.uploader.setOptions({ url: this.baseUrl + 'projects/' + this.username + '/' + this.projectname + '/add-project-photo' });

    this.memberService.createProject(this.model).subscribe({
      next: _ => {
        this.uploader.uploadAll();
      },
      error: error => {
        this.toastr.error(error.error);
      }
    })
  }

  fileOverBase(e: any) {
    this.HasBaseDropZoneOver = e;
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
        this.toastr.info("An image has been successfully uploaded!");
      }
    }

    this.uploader.onCompleteAll = () => {
      this.projectForm.reset(this.model);
      this.toastr.success("Project created successfully!");
      this.router.navigateByUrl('/' + this.user.username);
    };
  }
}
