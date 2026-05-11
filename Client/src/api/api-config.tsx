import { Platform } from 'react-native';

const getBaseUrl = (): string => {
    const API_VERSION = 'v1';
    const PLATFORM_PATH = 'mobile';

    if (__DEV__) {
        if (Platform.OS === 'android') {
            return `http://192.168.0.10:8080/api/${API_VERSION}/${PLATFORM_PATH}`;
        } else if (Platform.OS === 'ios') {
            return `http://localhost:8080/api/${API_VERSION}/${PLATFORM_PATH}`;
        }
    }

    return `https://your-production-api.com/api/${API_VERSION}/${PLATFORM_PATH}`;
};

export const API_BASE_URL = getBaseUrl();

// Debug - sprawdź w logach
console.log('API_BASE_URL:', API_BASE_URL);