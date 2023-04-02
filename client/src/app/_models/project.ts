import { ProjectPhoto } from "./projectphoto";

export interface Project {
    id: number;
    mainPhotoURL: string;
    projectName: string;
    projectTitle: string;
    language: string;
    description: string;
    projectPhotos: ProjectPhoto[];
}
