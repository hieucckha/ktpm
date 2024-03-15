import localStorageService from "./localStorage.service";

const notificationService = {
    addNotification: (message: object) => {
        const notifications = notificationService.getNotifications() ?? [];
        localStorageService.setItem("notifications", JSON.stringify([message, ...notifications]));
    },

    getNotifications:() :object[] => {
        const notifications = localStorageService.getItem("notifications") as string ?? "[]";
        return JSON.parse(notifications) as object[];
    },

    clear() {
        localStorageService.removeItem("notifications");
    }
}


export default notificationService;