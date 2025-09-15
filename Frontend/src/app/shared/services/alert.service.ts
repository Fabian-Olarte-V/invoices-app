import { Injectable } from '@angular/core';
import { Alert, AlertType } from '../interfaces/alert.interface';

@Injectable({
    providedIn: 'root'
})
export class AlertService {

    private showAlert(alert: Alert): void {
        window.alert(`${alert.type.toUpperCase()}: ${alert.message}`);
    }

    success(message: string): void {
        this.showAlert({
            type: AlertType.SUCCESS,
            message
        });
    }

    error(message: string): void {
        this.showAlert({
            type: AlertType.ERROR,
            message
        });
    }
}