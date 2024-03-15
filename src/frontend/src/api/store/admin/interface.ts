export interface ClassListQuery {
	id: number;
	name: string;
	description: string;
    isActivated:	boolean;
    createdAt: string;
    updatedAt: string;
    creatorId: number;
    creatorName: string;
    numberOfStudents: number;
    numberOfTeachers: number;
}
export interface userListQuery {
    id: number;
    fullName: string;
    role: string;
    status: string;
    lockoutEnd:string;
    email:string;
    firstName:string;
    lastName:string;
    studentId:string;
}