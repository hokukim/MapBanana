import { Action, createReducer, on } from '@ngrx/store';
import { IAppState, ICampaignsState } from 'src/app/app.store';
import * as CampaignActions from './campaign.actions';
import { ICampaign } from './campaign.model';

export const initialState: ICampaignsState = {
    campaigns: new Map<string, ICampaign>()
};

export const campaignReducer = createReducer(
    initialState,
    on(CampaignActions.setCampaigns, (state, { campaigns }) => ({
        campaigns: state.campaigns
    }))
  );