export interface ClassDto {
	name: string;
	description: string;
}
export interface ClassQuery {
	id: number;
	name: string;
	description: string;
}
export interface Teacher{
	id: number;
	fullName: string;
	role: string;
}
export interface Student  {
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
	createdAt: string;
	updatedAt: string;
}
export interface requests{
	id: number;
	studentId: number;
	studentName: string;
	classId: number;
	reason: string;
	createdAt: string;
	updatedAt: string;

}
export interface ClassDetail {
	id: number;
	name: string;
	description: string;
	inviteCode: string;
	inviteLink: string;
	numberOfStudents: number;
	numberTeacher: number;
	teachers: Teacher[];
	student: Student[];
	gradeCompositions: gradeCompositions[];
	requests: requests[];

}