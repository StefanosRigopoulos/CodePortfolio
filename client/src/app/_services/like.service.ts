import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Project } from '../_models/project';

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getLikedProjects() {
    return this.http.get<Project[]>(this.baseUrl + 'like');
  }

  likeUpdate(username: string, projectname: string) {
    return this.http.post(this.baseUrl + 'like/' + username + '/' + projectname, projectname);
  }
}
