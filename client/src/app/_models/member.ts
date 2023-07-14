import { Photo } from "./photo";
import { Project } from "./project";

export interface Member {
    id: number;
    userName: string;
    nickName: string;
    email: string;
    age: number;
    created: Date;
    lastActive: Date;
    firstName: string;
    lastName: string;
    gender: string;
    country: string;
    codeLanguage: string;
    profilePhotoURL: string;
    photo: Photo;
    projects: Project[];
    likedProjects: Project[];
    bio: string;
    facebookURL: string;
    twitterURL: string;
    gitHubURL: string;
    linkedInURL: string;
}


