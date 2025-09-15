export enum AlertType {
    SUCCESS = 'success',
    ERROR = 'error',
}

export interface Alert {
    type: AlertType;
    message: string;
}