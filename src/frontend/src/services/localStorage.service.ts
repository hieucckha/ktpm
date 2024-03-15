const localStorageService = {
	setItem(key: string, value: string) {
		try {
			localStorage.setItem(key, JSON.stringify(value));
		} catch (error) {
			console.error(`Error setting ${key} in localStorage:`, error);
		}
	},

	getItem(key: string) {
		try {
			const storedValue = localStorage.getItem(key);
			return storedValue ? JSON.parse(storedValue) : null;
		} catch (error) {
			console.error(`Error getting ${key} from localStorage:`, error);
			return null;
		}
	},

	removeItem(key: string) {
		try {
			localStorage.removeItem(key);
		} catch (error) {
			console.error(`Error removing ${key} from localStorage:`, error);
		}
	},
};

export default localStorageService;
