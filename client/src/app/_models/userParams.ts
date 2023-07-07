import { User } from "./user";

export class UserParams {
    codeLanguage: string = "";
    pageNumber = 1;
    pageSize = 8;
    orderBy = 'lastActive';

    constructor(user: User) { }
}