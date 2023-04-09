import { HttpClient, HttpParams } from '@angular/common/http';
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

  updateProject(username: string, projectname: string, project: Project) {
    return this.http.put(this.baseUrl + 'projects/' + username + '/' + projectname + '/edit', project)
  }

  setMainProjectPhoto(username: string, projectname: string, photoId: number) {
    return this.http.put(this.baseUrl + 'projects/' + username + '/' + projectname + '/set-main-photo/' + photoId, photoId)
  }

  deleteProjectPhoto(username: string, projectname: string, photoId: number) {
    return this.http.delete(this.baseUrl + 'projects/' + username + '/' + projectname + '/delete-photo/' + photoId)
  }
}
