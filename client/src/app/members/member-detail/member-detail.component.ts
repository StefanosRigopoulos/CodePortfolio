import { Component, OnInit } from '@angular/core';
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

  constructor(private memberService: MembersService, private router: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    var username = this.router.snapshot.paramMap.get('username');
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