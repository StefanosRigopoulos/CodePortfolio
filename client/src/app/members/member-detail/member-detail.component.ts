import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { Project } from 'src/app/_models/project';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})

export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
  member: Member | undefined;
  projects: Project[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];

  constructor(private memberService: MembersService, private messageService: MessageService, private router: ActivatedRoute) { }

  ngOnInit(): void {
    this.router.data.subscribe({
      next: data => this.member = data['member']
    })
    this.projects = this.member.projects;

    this.router.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      this.memberTabs.tabs.find(x => x.heading === heading)!.active = true;
    }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages') {
      this.loadMessages();
    }
  }

  loadMessages() {
    if (this.member) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: message => this.messages = message
      })
    }
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