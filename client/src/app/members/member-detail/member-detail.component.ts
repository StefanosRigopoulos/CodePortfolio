import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { Project } from 'src/app/_models/project';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})

export class MemberDetailComponent implements OnInit {
  member: Member | undefined;
  projects: Project[] = [];
  contactForm: FormGroup = new FormGroup({});

  constructor(private memberService: MembersService, private route: ActivatedRoute, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    var username = this.route.snapshot.paramMap.get('username');
    if (!username) return;
    this.memberService.getMember(username).subscribe({
      next: member => {
        this.member = member,
        this.projects = member.projects
      }
    })
  }

  facebookSet() {
    if (this.member.facebookURL == null) return false;
    return true;
  }

  twitterSet() {
    if (this.member.twitterURL == null) return false;
    return true;
  }

  githubSet() {
    if (this.member.gitHubURL == null) return false;
    return true;
  }

  linkedinSet() {
    if (this.member.linkedInURL == null) return false;
    return true;
  }
}