import axios from "../api/AxiosClient";
import { ClassListQuery, userListQuery } from "../api/store/admin/interface";
import { editUserDto } from "../api/store/auth/interface";

import {
	RequestStatus,
	type ClassDetail,
	type ClassDto,
	type ClassQuery,
	type EditGradeDto,
	type gradeCompositions,
	type newGradeCompositions,
	EditUserDto,
} from "../api/store/class/interface";

const classService = {
	async createClass(classData: ClassDto) {
		const response = await axios.post("/api/course", classData);
		return response.data;
	},
	async getAllClass(userId: Number): Promise<Array<ClassQuery>> {
		const response = await axios.get("/api/course/query", {
			params: { userId: userId },
		});

		return response.data.items;
	},
	async getAllClassWithoutParams(): Promise<Array<ClassListQuery>> {
		const response = await axios.get("/api/course/query");

		return response.data.items;
	},
	async getClassDetail(classId: String): Promise<ClassDetail> {
		const response = await axios.get(`/api/course/${classId}`);
		return response.data;
	},

	async editClass(classQuery: ClassQuery) {
		const response = await axios.patch(`/api/course/${classQuery.id}`, {
			name: classQuery.name,
			description: classQuery.description,
		});

		return response.data;
	},
	async joinClassByCode(classData: { code: string }) {
		const response = await axios.post("/api/course/join", {
			inviteCode: classData.code,
		});
		return response.data;
	},
	async joinCourseByInvitationLink(token: string) {
		const response = await axios.post("/api/course/invite-email/confirm", {
			token: token,
		});
		return response;
	},
	async joinCourseByCode(code: string) {
		const response = await axios.post("/api/course/join", { inviteCode: code });
		return response;
	},
	async updateOrderGradeComposit(gradeCompositions: gradeCompositions[]) {
		const response = await axios.patch("/api/grade-composition/sort", {
			gradeCompositions: gradeCompositions,
		});
		return response.data;
	},
	async updateGradeColumn(gradeComposition: gradeCompositions) {
		const response = await axios.put(
			"/api/grade-composition/" + gradeComposition.id,
			{
				name: gradeComposition.name,
				description: gradeComposition.description,
				gradeScale: gradeComposition.gradeScale,
			}
		);
		return response.data;
	},

	async editGradeStudent(body: EditGradeDto) {
		const response = await axios.post("/api/grade", body);
		return response.data;
	},

	async addNewGradeComposit(composition: newGradeCompositions) {
		const response = await axios.post("/api/grade-composition", {
			gradeScale: composition.gradeScale,
			name: composition.name,
			courseId: composition.courseId,
			description: composition.description,
		});
		return response.data;
	},
	async deleteGradeComposit(id: Number) {
		const response = await axios.delete("/api/grade-composition/" + id);
		return response.data;
	},
	async inviteClassByEmail(classData: { email: string; courseId: string }) {
		const response = await axios.post(
			`api/course/${classData.courseId}/invite-email`,
			{ email: classData.email }
		);
		return response.data;
	},

	async downloadGradeTemplate(classId: string) {
		if (!classId || classId === "") throw new Error("classId is required");
		const response = await axios.get(`/api/grade/template`, {
			params: { courseId: classId },
			responseType: "blob",
		});
		return response;
	},
	async uploadGradeFile(classId: string, file: File) {
		if (!classId || classId === "") throw new Error("classId is required");
		const formData = new FormData();
		formData.append("file", file);
		const response = await axios.post(
			`/api/grade/template/${classId}/import`,
			formData,
			{ headers: { "Content-Type": "multipart/form-data" } }
		);
		return response.data;
	},
	async getAllGrade(classId: string) {
		if (!classId || classId === "") throw new Error("classId is required");
		const response = await axios.get(`/api/grade/all`, {
			params: { courseId: classId },
		});

		return response.data;
	},
	async importStudent(classId: string, file: File) {
		if (!classId || classId === "") throw new Error("classId is required");
		const formData = new FormData();
		formData.append("file", file);
		const response = await axios.post(
			`/api/grade/student/template/${classId}/import`,
			formData,
			{ headers: { "Content-Type": "multipart/form-data" } }
		);
		return response.data;
	},

	async downloadTemplateImportStudent(classId: string) {
		return axios.get(`/api/grade/student/tempalte`, {
			params: { CourseId: classId },
			responseType: "blob",
		});
	},

	async getOneGradeStudent(classId: string, studentId: string) {
		if (!classId || classId === "") throw new Error("classId is required");
		const response = await axios.get(
			`/api/grade/course/${classId}/student/${studentId}`
		);
		return response.data;
	},

	async approveGradeComposition(id: Number) {
		const response = await axios.put(`/api/grade-composition/${id}/final`);
		return response.data;
	},

	async toggleActivateClass(classId: string) {
		if (!classId || classId === "") throw new Error("classId is required");
		const response = await axios.post(`/api/course/${classId}/activate`);
		return response.data;
	},
	async downloadTemplateImportAllStudent() {
		return axios.get(`/api/user/student/template`, {
			responseType: "blob",
		});
	},
	async uploadAllStudentId(file: File) {
		const formData = new FormData();
		formData.append("file", file);
		const response = await axios.post(
			`/api/user/student`,
			formData,
			{ headers: { "Content-Type": "multipart/form-data" } }
		);
		return response.data;
	},
	async userQueryResult(): Promise<Array<userListQuery>> {
		const response = await axios.get("/api/user/search");

		return response.data.items;
	},
	async lockUser(userId: Number) {
		const response = await axios.put(`/api/user/${userId}/ban`);
		return response.data;
	},
	async unlockUser(userId: Number) {
		const response = await axios.put(`/api/user/${userId}/unban`);
		return response.data;
	},
	async editUser(id:number, body: Omit<EditUserDto, "id">) {
		const response = await axios.put(`/api/user/${id}`, body);
		return response.data;
	},
	async downloadStudentGrade(classId: string) {
		return axios.get(`/api/grade/all/export`, {
			params: { CourseId: classId },
			responseType: "blob",
		});
	},
};
export default classService;

// Function to generate an array of data
function genArrayData(count) {
	const dataArray = [];

	for (let i = 1; i <= count; i++) {
		const data = {
			id: i,
			studentId: i + 1000,
			gradeId: (i % 3) + 1,
			studentName: `Student ${i}`,
			currentGrade: Math.floor(Math.random() * 100) + 1,
			expectedGrade: Math.floor(Math.random() * 100) + 1,
			classId: (i % 5) + 1,
			reason: `Reason for request ${i}`,
			createdAt: getCurrentDayFormatted(),
			updatedAt: getCurrentDayFormatted(),
			status: RequestStatus.Rejected,
		};

		dataArray.push(data);
	}

	return dataArray;
}
export const getCurrentDayFormatted = () => {
	const today = new Date();
	const yyyy = today.getFullYear();
	let mm = today.getMonth() + 1; // Months start at 0!
	let dd = today.getDate();

	var day = dd >= 10 ? dd.toString() : "0" + dd;
	var month = mm >= 10 ? mm.toString() : "0" + mm;
	const formattedToday = day + "/" + month + "/" + yyyy;
	return formattedToday;
};
