import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Project } from 'src/app/_models/project';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { ProjectsService } from 'src/app/_services/projects.service';

@Component({
  template: 'The href is: {{href}}',
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.css']
})
export class ProjectDetailsComponent implements OnInit {
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  model: any = {};
  project: Project;

  pieces: string[] = [];
  username: string;
  projectname: string;

  constructor(private projectService: ProjectsService, public accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.pieces = this.router.url.split('/');
    this.username = this.pieces[1];
    this.projectname = this.pieces[3];
    this.loadProject();

    this.galleryOptions = [
      {
        width: '800px',
        height: '800px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]

    this.galleryImages = this.getImages();
  }

  getImages(){
    if (!this.project) return;
    const imageUrls = [];
    for (const photo of this.project.projectPhotos){
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url
      })
    }
    return imageUrls;
  }

  loadProject() {
    if (!this.username || !this.projectname) return;
    this.projectService.getProject(this.username, this.projectname).subscribe({
      next: project => {
        this.model = project,
        this.project = project,
        this.galleryImages = this.getImages();
      }
    })
  }
}
