import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Project } from 'src/app/_models/project';

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.css']
})
export class ProjectCardComponent implements OnInit {
  @Input() project: Project | undefined;
  @Input() member: Member | undefined;

  constructor() { }

  ngOnInit(): void {
  }
}
