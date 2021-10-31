import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { setCampaigns } from "../store/campaigns/campaigns.actions";
import { ICampaign } from "../store/campaigns/campaigns.model";
import { setMaps } from "../store/maps/maps.actions";
import { IMap } from "../store/maps/maps.model";


@Injectable({
    providedIn: 'root'
})
export class BananaApiService{

    private bananaHubUrl: string = 'https://localhost:5001/api/campaign';

    public constructor(
        private httpClient: HttpClient,
        private store: Store) { }

    public getCampaigns(): void {
        const url: string = `${this.bananaHubUrl}/campaigns`;
        this.httpClient.get<Map<string, ICampaign>>(url).subscribe(response => {
            this.store.dispatch(setCampaigns({ campaigns: response }));
        });
    }

    public addCampaign(name: string): void {
        const url: string = `${this.bananaHubUrl}`;

        const data = {
            "Name": name
        };

        this.httpClient.post(url, data).subscribe(response => {
            console.log(response);
        });
    }

    public getMaps(campaignId: string): void {
        const url: string = `${this.bananaHubUrl}/${campaignId}/maps`;

        this.httpClient.get<Map<string, IMap>>(url).subscribe(response => {
            this.store.dispatch(setMaps({ maps: response }));
        });
    }

    public addMap(campaignId: string, file: File) {
        const url: string = `${this.bananaHubUrl}/${campaignId}/map`;

        const data: FormData = new FormData();

        if (!file) {
            return;
        }

        data.append("file", file);
        this.httpClient.post<IMap>(url, data).subscribe();
    }
}