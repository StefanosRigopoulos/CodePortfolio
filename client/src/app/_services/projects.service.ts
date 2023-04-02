import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Project } from '../_models/project';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getProjects(username: string) {
    return this.http.get<Project[]>(this.baseUrl + 'projects/' + username)
  }

  getProject(username: string, projectname: string) {
    return this.http.get<Project>(this.baseUrl + 'projects/' + username + '/' + projectname)
  }
}
