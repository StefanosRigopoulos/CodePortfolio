import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { Project } from 'src/app/_models/project';
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
  project: Project | undefined;
  pieces: string[] = [];

  constructor(private projectService: ProjectsService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.pieces = this.router.url.split('/');
    this.loadProject(this.pieces[2], this.pieces[3]);

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

  loadProject(username: string, projectName: string) {
    if (!username || !projectName) return;
    this.projectService.getProject(username, projectName).subscribe({
      next: project => {
        this.project = project,
        this.galleryImages = this.getImages();
      }
    })
  }
}
