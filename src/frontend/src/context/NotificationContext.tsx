import React, { createContext, useState } from 'react'
import notificationService from '../services/notification.service';

export const NotificationContext = createContext({} as any);

export const NotificationProvider = ({ children }) => {

    const [notifications, setNotifications] = useState(notificationService.getNotifications());

    return (
        <NotificationContext.Provider value={{ notifications, setNotifications }}>
            {children}
        </NotificationContext.Provider>
    )
}