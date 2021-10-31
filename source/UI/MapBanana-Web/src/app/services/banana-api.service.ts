import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { IAppState } from "../app.state";
import { setCampaigns } from "../store/campaigns/campaigns.actions";
import { ICampaign } from "../store/campaigns/campaigns.model";


@Injectable({
    providedIn: 'root'
})
export class BananaHubService{

    private bananaHubUrl: string = 'https://localhost:5001/api';

    public constructor(
        private httpClient: HttpClient,
        private store: Store) { }

    public getCampaigns(): void {
        const url: string = `${this.bananaHubUrl}/campaign/campaigns`;
        this.httpClient.get<Map<string, ICampaign>>(url).subscribe(response => {
            console.log(response);
            this.store.dispatch(setCampaigns({ campaigns: response }));
        });
    }

    public addCampaign(name: string): void {
        const url: string = `${this.bananaHubUrl}/campaign`;

        const data = {
            "Name": name
        };

        this.httpClient.post(url, data).subscribe(response => {
            console.log(response);
        });
    }
}