import { UserProfileDto } from "../auth/interface";

export interface ClassDto {
	name: string;
	description: string;
}
export interface ClassQuery {
	id: number;
	name: string;
	description: string;
	isActivated?: boolean;
}
export interface Teacher {
	id: number;
	fullName: string;
	role: string;
}
export interface Student {
	id: number;
	fullName: string;
	role: string;
}
export interface gradeCompositions {
	id: number;
	key: number;
	name: string;
	courseId: number;
	description: string;
	gradeScale: number;
	order: number;
	isFinal?: boolean;
	createdAt: string;
	updatedAt: string;
}
export interface newGradeCompositions {
	name: string;
	courseId: number;
	description: string;
	gradeScale: number;
}

export enum RequestStatus {
	Pending = "Pending",
	Approved = "Approved",
	Rejected = "Rejected",
}

export interface CommentDto {
	id: number;
	requestId: number;
	userId: string;
	name?: string;
	comment?: string;
	isTeacher: boolean;
	createdAt: string;
	updatedAt: string;
}

export interface RequestDto {
	id: number;
	studentId: number;
	studentName?: string;
	gradeId: number;
	gradeName?: string;
	currentGrade: number;
	expectedGrade: number;
	reason?: string;
	status: RequestStatus;
	comments?: CommentDto[];
	createdAt: string;
	updatedAt: string;
}

export interface ClassDetail {
	id: number;
	name: string;
	description: string;
	inviteCode: string;
	inviteLink: string;
	isActivated?: boolean;
	numberOfStudents: number;
	numberTeacher: number;
	teachers: Teacher[];
	students: Student[];
	gradeCompositions: gradeCompositions[];
	requests: RequestDto[];
}
export interface gradeCompositionsDto {
	id: number;
	key: number;
	name: string;
	courseId: number;
	description: string;
	isFinal: boolean;
	gradeScale: number;
	order: number;
	createdAt: string;
	updatedAt: string;
}
export interface GradeDTO {
	id: number;
	gradeCompositionId: number;
	gradeValue: number;
	isRequest: boolean;
}
export interface studentGradeDto {
	studentId: number;
	studentName: string;
	userId: number;
	gradeDto: GradeDTO[];
}
export interface gradeAll {
	courseId: number;
	gradeCompositionDtos: gradeCompositionsDto[];
	students: studentGradeDto[];
}

export interface EditGradeDto {
	gradeCompositionId: number;
	studentId: string;
	gradeValue: number;
}

export interface EditUserDto extends Partial<Pick<UserProfileDto, "firstName" | "lastName" | "studentId" | "email">> {
	id: number;
}
