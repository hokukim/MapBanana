import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ICampaign } from "../store/campaign/campaign.model";


@Injectable({
    providedIn: 'root'
})
export class BananaHubService{

    private bananaHubUrl: string = 'https://localhost:5001/api';

    public constructor(private httpClient: HttpClient) { }

    public getCampaigns(): ICampaign[] {
        const url: string = `${this.bananaHubUrl}/campaigns`;
        this.httpClient.get(url).subscribe(response => {
        });
    }
}