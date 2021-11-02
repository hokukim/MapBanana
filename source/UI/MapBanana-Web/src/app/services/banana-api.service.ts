import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { setCampaign } from "../store/campaign/campaign.actions";
import { setCampaigns } from "../store/campaigns/campaigns.actions";
import { ICampaign } from "../store/campaigns/campaigns.model";
import { setMap } from "../store/map/map.actions";
import { setMaps } from "../store/maps/maps.actions";
import { IMap } from "../store/maps/maps.model";


@Injectable({
    providedIn: 'root'
})
export class BananaApiService{

    private bananaHubUrl: string = 'https://localhost:5001/api';
    private campaignUrl: string = `${this.bananaHubUrl}/campaign`;
    private mapUrl: string = `${this.bananaHubUrl}/map`;

    public constructor(
        private httpClient: HttpClient,
        private store: Store) { }

    public getCampaigns(): void {
        const url: string = `${this.campaignUrl}/campaigns`;
        this.httpClient.get<Map<string, ICampaign>>(url).subscribe(response => {
            this.store.dispatch(setCampaigns({ campaigns: response }));
        });
    }

    public addCampaign(name: string): void {
        const url: string = `${this.campaignUrl}`;

        const data = {
            "Name": name
        };

        this.httpClient.post(url, data).subscribe();
    }

    public getMaps(campaignId: string): void {
        const url: string = `${this.campaignUrl}/${campaignId}/maps`;

        this.httpClient.get<Map<string, IMap>>(url).subscribe(response => {
            this.store.dispatch(setMaps({ maps: response }));
        });
    }

    public addMap(campaignId: string, file: File, smallFile: File) {
        const url: string = `${this.campaignUrl}/${campaignId}/map`;

        const data: FormData = new FormData();

        // Add map image file.
        if (!file) {
            return;
        }

        data.append("file", file);

        // Add map small image file.
        if (smallFile) {
            data.append("smallFile", smallFile);
        }

        this.httpClient.post<IMap>(url, data).subscribe();
    }

    public getMap(mapId: string) {
        const url: string = `${this.mapUrl}/${mapId}`;

        this.httpClient.get<IMap>(url).subscribe(response => {
            this.store.dispatch(setMap({ map: response }));
        });
    }

    public activateMap(campaignId: string, mapId: string) {
        const url: string = `${this.campaignUrl}/${campaignId}/activeMap`;

        const data = {
            "Id": mapId
        };

        this.httpClient.post(url, data).subscribe();
    }

    public getCampaign(campaignId: string) {
        const url: string = `${this.campaignUrl}/${campaignId}`;

        this.httpClient.get<ICampaign>(url).subscribe(response => {
            this.store.dispatch(setCampaign({ campaign: response }));
        });        
    }
}