export const convertBlobToJson = (blob: any):any => {
	let file = new Blob([blob], { type: "application/json" });

    return file.text().then((value) => {
		const objectName = JSON.parse(value);
        return objectName;

	});
};
