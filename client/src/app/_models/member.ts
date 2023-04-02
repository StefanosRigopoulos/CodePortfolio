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
    projects: Project[];
    bio: string;
    facebookURL: string;
    twitterURL: string;
    gitHubURL: string;
    linkedInURL: string;
}


